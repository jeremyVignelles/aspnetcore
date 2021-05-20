// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Language
{
    internal readonly ref struct StringTokenizer
    {
        private readonly ReadOnlySpan<char> _value;
        private readonly char _separators;

        public StringTokenizer(ReadOnlySpan<char> value, char separators)
        {
            _value = value;
            _separators = separators;
        }

        public Enumerator GetEnumerator() => new Enumerator(in _value, _separators);

        public ref struct Enumerator
        {
            private readonly char _separator;
            private ReadOnlySpan<char> _value;

            internal Enumerator(in ReadOnlySpan<char> value, char separator)
            {
                _value = value;
                _separator = separator;
                Current = default;
            }

            public ReadOnlySpan<char> Current { get; private set; }

            public bool MoveNext()
            {
                if (_value.IsEmpty)
                {
                    Current = default;
                    return false;
                }

                var next = _value.IndexOf(_separator);
                if (next == -1)
                {
                    // No separator found. Consume the remainder of the string.
                    Current = _value;
                    _value = default;
                }
                else
                {
                    Current = _value.Slice(0, next);
                    _value = _value.Slice(next + 1);
                }

                return true;
            }
        }
    }
}
