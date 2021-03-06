#region License
//
// Copyright 2002-2015 Drew Noakes
// Ported from Java to C# by Yakov Danilov for Imazen LLC in 2014
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// More information about this project is available at:
//
//    https://github.com/drewnoakes/metadata-extractor-dotnet
//    https://drewnoakes.com/code/exif/
//
#endregion

using MetadataExtractor.Formats.Jfif;
using MetadataExtractor.IO;
using Xunit;

namespace MetadataExtractor.Tests.Formats.Jfif
{
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public sealed class JfifReaderTest
    {
        [Fact]
        public void TestRead()
        {
            var jfifData = new byte[] { 74, 70, 73, 70, 0, 1, 2, 1, 0, 108, 0, 108, 0, 0 };

            var directory = new JfifReader().Extract(new ByteArrayReader(jfifData));

            Assert.NotNull(directory);
            Assert.False(directory.HasError, directory.Errors.ToString());

            var tags = directory.Tags;

            Assert.Equal(6, tags.Count);
            Assert.Equal(JfifDirectory.TagVersion, tags[0].TagType);
            Assert.Equal(0x0102, directory.GetInt32(tags[0].TagType));
            Assert.Equal(JfifDirectory.TagUnits, tags[1].TagType);
            Assert.Equal(1, directory.GetInt32(tags[1].TagType));
            Assert.Equal(JfifDirectory.TagResX, tags[2].TagType);
            Assert.Equal(108, directory.GetInt32(tags[2].TagType));
            Assert.Equal(JfifDirectory.TagResY, tags[3].TagType);
            Assert.Equal(108, directory.GetInt32(tags[3].TagType));
            Assert.Equal(JfifDirectory.TagThumbWidth, tags[4].TagType);
            Assert.Equal(0, directory.GetInt32(tags[4].TagType));
            Assert.Equal(JfifDirectory.TagThumbHeight, tags[5].TagType);
            Assert.Equal(0, directory.GetInt32(tags[5].TagType));
        }
    }
}
