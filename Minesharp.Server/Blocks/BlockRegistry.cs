using Minesharp.Server.Blocks.Type;
using Newtonsoft.Json.Linq;

namespace Minesharp.Server.Blocks;

public class BlockRegistry
{
    private readonly Dictionary<Material, int> blockIds = new();
    private readonly Dictionary<int, Material> materials = new();

    private readonly Dictionary<Material, BlockType> blockTypes = new();

    public void LoadBlocksMapping()
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

                    blockIds[material] = id;
                    materials[id] = material;
                }
            }
        }
    }
    
    public void LoadBlockTypes()
    {
        var blocks = typeof(BlockType).Assembly.GetTypes()
            .Where(x => typeof(BlockType).IsAssignableFrom(x))
            .Where(x => !x.IsAbstract);

        foreach (var type in blocks)
        {
            var block = Activator.CreateInstance(type) as BlockType;
            if (block is null)
            {
                continue;
            }

            foreach (var material in block.Types)
            {
                blockTypes[material] = block;
            }
        }
    }

    public int GetBlockId(Material material)
    {
        return blockIds.GetValueOrDefault(material);
    }

    public Material GetMaterial(int blockType)
    {
        return materials.GetValueOrDefault(blockType);
    }

    public BlockType GetBlock(Material material)
    {
        return blockTypes.GetValueOrDefault(material);
    }
}