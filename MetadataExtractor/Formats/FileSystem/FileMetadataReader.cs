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

using System.IO;
using JetBrains.Annotations;

#if WINRT
using System;
using Windows.Storage;
using System.Threading.Tasks;
#endif

namespace MetadataExtractor.Formats.FileSystem
{
    public sealed class FileMetadataReader
    {
#if WINRT
        public async Task<FileMetadataDirectory> ReadAsync([NotNull]StorageFile file)
        {
            var attr = await file.GetBasicPropertiesAsync().AsTask().ConfigureAwait(false);

            var directory = new FileMetadataDirectory();

            directory.Set(FileMetadataDirectory.TagFileName, Path.GetFileName(file.Path));
            directory.Set(FileMetadataDirectory.TagFileSize, attr.Size);
            directory.Set(FileMetadataDirectory.TagFileModifiedDate, attr.DateModified);

            return directory;
        }
#else
        /// <exception cref="System.IO.IOException"/>
        public FileMetadataDirectory Read([NotNull] string file)
        {
            var attr = File.GetAttributes(file);

            if ((attr & FileAttributes.Directory) != 0)
                throw new IOException("File object must reference a file");

            var fileInfo = new FileInfo(file);

            if (!fileInfo.Exists)
                throw new IOException("File does not exist");

            var directory = new FileMetadataDirectory();

            directory.Set(FileMetadataDirectory.TagFileName, Path.GetFileName(file));
            directory.Set(FileMetadataDirectory.TagFileSize, fileInfo.Length);
            directory.Set(FileMetadataDirectory.TagFileModifiedDate, fileInfo.LastWriteTime);

            return directory;
        }
#endif
    }
}
