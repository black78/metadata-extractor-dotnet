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

using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

#if WINRT
using System.Threading.Tasks;
using Windows.Storage;
#endif

namespace MetadataExtractor.Formats.Exif
{
    /// <summary>One of several Exif directories.</summary>
    /// <remarks>Otherwise known as IFD1, this directory holds information about an embedded thumbnail image.</remarks>
    /// <author>Drew Noakes https://drewnoakes.com</author>
    public sealed class ExifThumbnailDirectory : ExifDirectoryBase
    {
        /// <summary>The offset to thumbnail image bytes.</summary>
        public const int TagThumbnailOffset = 0x0201;

        /// <summary>The size of the thumbnail image data in bytes.</summary>
        public const int TagThumbnailLength = 0x0202;

        private static readonly Dictionary<int, string> _tagNameMap = new Dictionary<int, string>
        {
            { TagThumbnailOffset, "Thumbnail Offset" },
            { TagThumbnailLength, "Thumbnail Length" }
        };

        static ExifThumbnailDirectory()
        {
            AddExifTagNames(_tagNameMap);
        }

        public ExifThumbnailDirectory()
        {
            SetDescriptor(new ExifThumbnailDescriptor(this));
        }

        public override string Name => "Exif Thumbnail";

        protected override bool TryGetTagName(int tagType, out string tagName)
        {
            return _tagNameMap.TryGetValue(tagType, out tagName);
        }

        public bool HasThumbnailData => ThumbnailData != null;

        [CanBeNull]
        public byte[] ThumbnailData { get; set; }

#if WINRT
        /// <exception cref="MetadataException"/>
        /// <exception cref="System.IO.IOException"/>
        public async Task WriteThumbnailAsync([NotNull] StorageFile file)
        {
            if (ThumbnailData == null)
                throw new MetadataException("No thumbnail data exists.");

            using (var stream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
            {
                await stream.WriteAsync(ThumbnailData, 0, ThumbnailData.Length);
            }
        }
#else
        /// <exception cref="MetadataException"/>
        /// <exception cref="System.IO.IOException"/>
        public void WriteThumbnail([NotNull] string filename)
        {
            if (ThumbnailData == null)
                throw new MetadataException("No thumbnail data exists.");

            File.WriteAllBytes(filename, ThumbnailData);
        }
#endif
    }
}
