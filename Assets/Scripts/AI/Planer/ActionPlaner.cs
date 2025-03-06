using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AI.Action;
using AI.Goal;
using Debug = UnityEngine.Debug;

public class PlanNode
{
    public ActorAction Action;
    public Dictionary<string, bool> DesiredState;
    public PlanNode Parent;
    public List<PlanNode> Children;
    public int Cost;
}

public class ActionPlaner
{
    public Queue<ActorAction> GetPlan(Goal goal, ActorAction[] actions, WorldState worldState)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        var desiredState = goal.GetDesiredState();

        var root = new PlanNode()
        {
            Action = null,
            DesiredState = desiredState,
            Children = new List<PlanNode>()
        };
        
        BuildGraph(root, actions, worldState);
        
        var plan = FindPlan(root, worldState);
        
        sw.Stop();
        
        return plan;
    }

    private Queue<ActorAction> FindPlan(PlanNode goalNode, WorldState worldState)
    {
        Dictionary<PlanNode, int> distances = new Dictionary<PlanNode, int>();
        Dictionary<PlanNode, PlanNode> previous = new Dictionary<PlanNode, PlanNode>();
        HashSet<PlanNode> visited = new HashSet<PlanNode>();
        
        var priorityQueue = new SortedSet<(int distance, PlanNode node)>(Comparer<(int, PlanNode)>.Create((a, b) =>
            a.Item1 == b.Item1 ? a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode()) : a.Item1.CompareTo(b.Item1)));

        distances[goalNode] = 0;
        priorityQueue.Add((0, goalNode));

        PlanNode startNode = null;
        
        while (priorityQueue.Count > 0)
        {
            var (currentDistance, node) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);
            
            if (visited.Contains(node)) continue;
            visited.Add(node);
            
            if (MatchesState(worldState.GetEffects(), node.DesiredState))
            {
                startNode = node;
                break;
            }
            
            foreach (var child in node.Children)
            {
                if (visited.Contains(child) || child.Action == null) continue;
                int newDist = currentDistance + child.Action.GetCost(worldState);
                if (!distances.ContainsKey(child) || newDist < distances[child])
                {
                    priorityQueue.Remove((distances.GetValueOrDefault(child, int.MaxValue), child));
                    distances[child] = newDist;
                    previous[child] = node;
                    priorityQueue.Add((newDist, child));
                }
            }
        }

        return startNode != null ? ReconstructPlan(startNode, previous) : new Queue<ActorAction>();
    }
    
    private bool MatchesState(Dictionary<string, bool> state, Dictionary<string, bool> goal)
    {
        return goal.All(kv => state.ContainsKey(kv.Key) && state[kv.Key] == kv.Value);
    }

    
    private Queue<ActorAction> ReconstructPlan(PlanNode startNode, Dictionary<PlanNode, PlanNode> previous)
    {
        var plan = new Queue<ActorAction>();
        for (PlanNode node = startNode; node != null; node = previous.GetValueOrDefault(node, null))
        {
            if (node.Action != null)
                plan.Enqueue(node.Action);
        }
        
        return plan;
    }
    
    private void BuildGraph(PlanNode root, ActorAction[] actions, WorldState worldState)
    {
        var desiredState = root.DesiredState;
        SetNodes(root, actions, desiredState, worldState);
    }

    private void SetNodes(
        PlanNode root, 
        ActorAction[] actions,
        Dictionary<string, bool> desiredState,
        WorldState worldState)
    {
        foreach (var action in actions)
        {
            if (root.Action == action) continue;
            
            var effects = action.Effects;
            if (IsSatisfiedConditions(desiredState, effects) && action.IsValid(worldState))
            {
                var node = new PlanNode
                {
                    Action = action,
                    Children = new List<PlanNode>(actions.Length),
                    Parent = root,
                    DesiredState = action.Preconditions,
                    Cost = action.GetCost(worldState)
                };
                
                root.Children.Add(node);

                SetNodes(node, actions, node.DesiredState, worldState);
            }
        }
    }

    private bool IsSatisfiedConditions(Dictionary<string, bool> conditions, Dictionary<string, bool> effects)
    {
        if (conditions == null || conditions.Count == 0) return false;

        return conditions.All(c => effects.ContainsKey(c.Key) && c.Value == effects[c.Key]);
    }
}
