using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XmlFormat;

namespace XmlFormat;

/// <summary>
/// A <see langword="ref"/> <see langword="struct"/> that tokenizes a given <see cref="ReadOnlySpan{char}"/> instance.
/// </summary>
public ref struct PredicatedReadOnlySpanTokenizer
{
    /// <summary>
    /// The source <see cref="ReadOnlySpan{char}"/> instance.
    /// </summary>
    private readonly ReadOnlySpan<char> span;

    /// <summary>
    /// The separator predicate to use.
    /// </summary>
    private readonly Func<char, bool> separator;

    /// <summary>
    /// The inverse of the separator predicate to use.
    /// </summary>
    private readonly Func<char, bool> notseparator;

    /// <summary>
    /// The current initial offset.
    /// </summary>
    private int start;

    /// <summary>
    /// The current final offset.
    /// </summary>
    private int end;

    /// <summary>
    /// Initializes a new instance of the <see cref="PredicatedReadOnlySpanTokenizer{char}"/> struct.
    /// </summary>
    /// <param name="span">The source <see cref="ReadOnlySpan{char}"/> instance.</param>
    /// <param name="separator">The separator item to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PredicatedReadOnlySpanTokenizer(ReadOnlySpan<char> span, Func<char, bool> separator)
    {
        this.span = span;
        this.separator = separator;
        this.notseparator = c => !separator(c);
        this.start = 0;
        this.end = -1;
    }

    /// <summary>
    /// Implements the duck-typed <see cref="IEnumerable{char}.GetEnumerator"/> method.
    /// </summary>
    /// <returns>An <see cref="PredicatedReadOnlySpanTokenizer{char}"/> instance targeting the current <see cref="ReadOnlySpan{char}"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly PredicatedReadOnlySpanTokenizer GetEnumerator() => this;

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
    /// </summary>
    /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        this.start = this.span.IndexOf(this.notseparator, this.end + 1);
        if (this.start < 0)
            return false;

        this.end = this.span.IndexOf(this.separator, this.start + 1);
        if (this.end < 0)
            this.end = this.span.Length;

        return true;
    }

    /// <summary>
    /// Gets the duck-typed <see cref="IEnumerator{char}.Current"/> property.
    /// </summary>
    public readonly ReadOnlySpan<char> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.span.Slice(this.start, this.end - this.start);
    }
}

public static class PredicatedReadOnlySpanTokenizerExtension
{
    public static int Count(this PredicatedReadOnlySpanTokenizer tokenizer)
    {
        int count = 0;
        foreach (var c in tokenizer)
            count++;

        return count;
    }
}
