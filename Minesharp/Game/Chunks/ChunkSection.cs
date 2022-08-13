// MIT License
// 
// Copyright (c) 2022 Roxeez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace Minesharp.Game.Chunks;

public sealed class ChunkSection
{
    private readonly ChunkPaletteMapping mapping;
    private readonly List<int> palette;

    public ChunkSection(int[] types)
    {
        palette = new HashSet<int>(types).ToList();
        mapping = new ChunkPaletteMapping(palette, types);

        BlockCount = types.Count(x => x is not 0);
    }

    public int BlockCount { get; }
    public byte Bits => mapping.Bits;
    public IList<int> Palette => palette;
    public IList<long> Mapping => mapping.Storage;
    public bool UsePalette => mapping.UsePalette;

    public int GetType(int x, int y, int z)
    {
        var value = mapping.Get(x, y, z);
        if (mapping.Bits < 8) // If bits > 8 we use global palette
        {
            value = palette[value];
        }

        return value;
    }
}