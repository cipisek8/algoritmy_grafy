using System.Text.Json.Nodes;

namespace Grafy;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter full file path: ");
        JsonObject obj;
        if (!IsGraphValid(Console.ReadLine(), out obj))
        {
            Console.WriteLine("Invalid graph");
            return;
        }

        bool isInputCorrect = false;
        string startId = null;
        string endId = null;
        string[] nodes = obj["nodes"].AsArray().Select(n => n.ToString()).ToArray();
        string nodesText = string.Join(", ", nodes);

        while (!isInputCorrect)
        {
            Console.Write($"Enter start node (from {nodesText}): ");
            string input = Console.ReadLine();
            if (nodes.All(n => n.ToString() != input))
            {
                Console.WriteLine("Invalid start node");
                continue;
            }
            startId = input;
            isInputCorrect = true;
        }

        isInputCorrect = false;
        nodes = nodes.Where(n => n.ToString() != startId).ToArray();
        nodesText = string.Join(", ", nodes);

        while (!isInputCorrect)
        {
            Console.Write($"Enter end node (from {nodesText}): ");
            string input = Console.ReadLine();
            if (nodes.All(n => n.ToString() != input))
            {
                Console.WriteLine("Invalid end node");
                continue;
            }
            endId = input;
            isInputCorrect = true;
        }

        double distance;
        string[] path = ShortestPath(obj, startId, endId, out distance);
        Console.WriteLine("Path: " + string.Join(", ", path));
        Console.WriteLine("Distance: " + distance);
    }

    public static bool IsGraphValid(string path, out JsonObject obj)
    {
        obj = null;
        try
        {
            var json = File.ReadAllText(path);
            var parsedObj = JsonNode.Parse(json);

            if (parsedObj is not JsonObject jsonObject || jsonObject["edges"] is not JsonObject || jsonObject["nodes"] is not JsonArray)
                return false;

            var edges = jsonObject["edges"].AsObject();
            var nodes = jsonObject["nodes"].AsArray();
            if (edges.Count == 0 || nodes.Count == 0)
                return false;

            foreach (var node in nodes)
            {
                if (node.ToString() == "")
                    return false;
            }

            foreach (var edge in edges)
            {
                if (edge.Value["nodeId1"] is null || edge.Value["nodeId2"] is null || edge.Value["value"] is null)
                    return false;

                if (nodes.All(n => n.ToString() != edge.Value["nodeId1"].ToString()) ||
                    nodes.All(n => n.ToString() != edge.Value["nodeId2"].ToString()))
                    return false;
            }

            obj = jsonObject;
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Bellman-Ford algoritm
    public static string[] ShortestPath(JsonObject graph, string startId, string targetId, out double totalDistance)
    {
        var nodes = graph["nodes"].AsArray();
        var edges = graph["edges"].AsObject();
        var nodeMap = nodes.ToDictionary(n => n.ToString(), n => n);
        var distance = new Dictionary<string, double>();
        var previous = new Dictionary<string, string>();

        foreach (var node in nodes)
        {
            distance[node.ToString()] = double.MaxValue;
            previous[node.ToString()] = null;
        }

        distance[startId] = 0;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            foreach (var edge in edges)
            {
                var u = edge.Value["nodeId1"].ToString();
                var v = edge.Value["nodeId2"].ToString();
                var w = double.Parse(edge.Value["value"].ToString());

                if (distance[u] + w < distance[v])
                {
                    distance[v] = distance[u] + w;
                    previous[v] = u;
                }
            }
        }

        foreach (var edge in edges)
        {
            var u = edge.Value["nodeId1"].ToString();
            var v = edge.Value["nodeId2"].ToString();
            var w = double.Parse(edge.Value["value"].ToString());

            if (distance[u] + w < distance[v])
                throw new Exception("Negative cycle detected");
        }

        var path = new List<string>();
        var currentNode = targetId;
        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = previous[currentNode];
        }

        totalDistance = distance[targetId];
        return targetId == startId || distance[targetId] == double.MaxValue ? null : path.ToArray();
    }
}

