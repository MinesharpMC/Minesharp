using Minesharp.Common.Enum;
using Newtonsoft.Json.Linq;

namespace Minesharp.Game.Blocks;

public class BlockRegistry
{
    private readonly Dictionary<Material, int> blockTypes = new();
    private readonly Dictionary<int, Material> materials = new();

    public void Load()
    {
        var root = JObject.Parse(File.ReadAllText("blocks.json"));
        foreach (var value in root.Children<JProperty>())
        {
            var name = value.Name;
            var states = value.Value.Value<JArray>("states")!;

            foreach (var state in states.Cast<JObject>())
            {
                var id = state.Value<int>("id");
                var isDefault = state.ContainsKey("default");

                if (isDefault)
                {
                    var material = Material.GetMaterial(name);
                    if (material is null)
                    {
                        continue;
                    }

                    blockTypes[material] = id;
                    materials[id] = material;
                }
            }
        }
    }

    public int GetBlockType(Material material)
    {
        return blockTypes.GetValueOrDefault(material);
    }

    public Material GetMaterial(int blockType)
    {
        return materials.GetValueOrDefault(blockType);
    }
}