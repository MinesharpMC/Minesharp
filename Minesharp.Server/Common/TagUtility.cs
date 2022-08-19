using Minesharp.Nbt;

namespace Minesharp.Server.Common;

public static class TagUtility
{
    public static CompoundTag CreateRegistryTag()
    {
        var registry = new CompoundTag
        {
            ["minecraft:dimension_type"] = new CompoundTag
            {
                ["type"] = new StringTag("minecraft:dimension_type"),
                ["value"] = new ListTag
                {
                    new CompoundTag
                    {
                        ["name"] = new StringTag("minecraft:overworld"),
                        ["id"] = new IntTag(0),
                        ["element"] = new CompoundTag
                        {
                            ["piglin_safe"] = new ByteTag(1),
                            ["has_raids"] = new ByteTag(0),
                            ["monster_spawn_light_level"] = new IntTag(10),
                            ["monster_spawn_block_light_limit"] = new IntTag(1),
                            ["natural"] = new ByteTag(0),
                            ["ambient_light"] = new FloatTag(0.5f),
                            ["infiniburn"] = new StringTag("#"),
                            ["respawn_anchor_works"] = new ByteTag(0),
                            ["has_skylight"] = new ByteTag(0),
                            ["bed_works"] = new ByteTag(0),
                            ["effects"] = new StringTag("minecraft:overworld"),
                            ["min_y"] = new IntTag(-64),
                            ["height"] = new IntTag(256),
                            ["logical_height"] = new IntTag(150),
                            ["coordinate_scale"] = new DoubleTag(0.00001),
                            ["ultrawarm"] = new ByteTag(0),
                            ["has_ceiling"] = new ByteTag(0)
                        }
                    }
                }
            },
            ["minecraft:worldgen/biome"] = new CompoundTag
            {
                ["type"] = new StringTag("minecraft:worldgen/biome"),
                ["value"] = new ListTag
                {
                    new CompoundTag
                    {
                        ["name"] = new StringTag("minecraft:plains"),
                        ["id"] = new IntTag(0),
                        ["element"] = new CompoundTag
                        {
                            ["precipitation"] = new StringTag("rain"),
                            ["depth"] = new FloatTag(1.5f),
                            ["temperature"] = new FloatTag(0.8f),
                            ["scale"] = new FloatTag(0.0f),
                            ["downfall"] = new FloatTag(0.4f),
                            ["category"] = new StringTag("plains"),
                            ["effects"] = new CompoundTag
                            {
                                ["sky_color"] = new IntTag(7907327),
                                ["water_fog_color"] = new IntTag(329011),
                                ["fog_color"] = new IntTag(12638463),
                                ["water_color"] = new IntTag(4159204)
                            }
                        }
                    }
                }
            },
            ["minecraft:chat_type"] = new CompoundTag
            {
                ["type"] = new StringTag("minecraft:chat_type"),
                ["value"] = new ListTag
                {
                    new CompoundTag
                    {
                        ["name"] = new StringTag("minecraft:system"),
                        ["id"] = new IntTag(0),
                        ["element"] = new CompoundTag
                        {
                            ["chat"] = new CompoundTag
                            {
                                ["chat"] = new CompoundTag(),
                                ["narration"] = new CompoundTag
                                {
                                    ["priority"] = new StringTag("system")
                                }
                            }
                        }
                    },
                    new CompoundTag
                    {
                        ["name"] = new StringTag("minecraft:game_info"),
                        ["id"] = new IntTag(1),
                        ["element"] = new CompoundTag
                        {
                            ["overlay"] = new CompoundTag()
                        }
                    }
                }
            }
        };

        return registry;
    }
}