using System;
using System.Collections.Generic;

namespace SalutemCRM.TCP;


public static partial class Extensions
{
    public static T? Cast<T>(this object obj) where T : class => obj as T;
    public static T? EnumCast<T>(this object obj) => (T)Enum.Parse(typeof(T), obj.ToString()!);
    public static T? DoIf<T>(this T obj, Action<T> action, Func<T, bool> predicate) { if (predicate(obj)) { action(obj); return obj; } return default(T?); }
    public static T? Do<T>(this T obj, Func<T, bool> action) => action(obj) ? obj : default(T?);
    public static T Do<T>(this T obj, Action<T> action) { action(obj); return obj; }
    public static T2 Do<T1, T2>(this T1 obj, Func<T1, T2> action) => action(obj);
    public static T1 DoInst<T1, T2>(this T1 obj, Func<T1, T2> action) { action(obj); return obj; }

    public static IEnumerable<T> DoForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
            action(item);
        return source;
    }
}