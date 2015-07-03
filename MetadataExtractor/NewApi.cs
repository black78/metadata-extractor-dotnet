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

// ReSharper disable CheckNamespace
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MetadataExtractor.NewApi
{
    public interface IDirectory : IEnumerable<IEntry>
    {
        string Name { get; }
        int Count { get; }
        IEnumerable<IDirectory> SubDirectories { get; }
    }

    public abstract class Directory<TKey, TEntry>
        : IDirectory
        where TEntry : IEntry
    {
        // TODO need to maintain order of values if we are to write data again

        private readonly Dictionary<TKey,TEntry> _entryByKey = new Dictionary<TKey, TEntry>();

        public string Name { get; private set; }

        protected Directory(string name)
        {
            Name = name;
        }

        public int Count
        {
            get { return _entryByKey.Count; }
        }

        public IEnumerable<IDirectory> SubDirectories
        {
            get { return _entryByKey.Values.Select(entry => entry.Value).OfType<IDirectory>(); }
        }

        public bool TryGetValue(TKey key, out TEntry entry)
        {
            return _entryByKey.TryGetValue(key, out entry);
        }

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IEntry> GetEnumerator()
        {
            return _entryByKey.Values.Cast<IEntry>().GetEnumerator();
        }

        #endregion
    }

    public interface IEntry
    {
        object Value { get; }
        string Description { get; }
        string Name { get; }
    }

    public enum ExifTag
    {
        Width,
        Height
    }

    public abstract class ExifDirectory : Directory<ExifTag, IEntry>
    {
        protected ExifDirectory(string name) : base(name)
        {}
    }

    public class ExifIfd0Directory : ExifDirectory
    {
        public ExifIfd0Directory()
            : base("Exif IFD0")
        {}

        public int Width
        {
            get { GetInt32 }
        }
    }

    public static class MetadataReader
    {
        public static IReadOnlyList<IDirectory> Read(string path)
        {}
    }

    public static class Program
    {
        private static void Main(string[] args)
        {
            var directories = MetadataReader.Read(args[0]);

            foreach (var directory in directories)
            foreach (var entry in directory)
                Console.Out.WriteLine("{0} - {1} = {2}", directory.Name, entry.Name, entry.Description);
        }
    }
}