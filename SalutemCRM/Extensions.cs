using System;

namespace SalutemCRM;

public delegate void SearchHandler(string keyword);

public static partial class Extensions
{
    public static T? Cast<T>(this object obj) where T : class => obj as T;

    public static T? DoIf<T>(this T obj, Action<T> action, Func<T, bool> state) { if (state(obj)) { action(obj); return obj; } return default(T?); }
    public static T? Do<T>(this T obj, Func<T, bool> action) => action(obj) ? obj : default(T?);
    public static T Do<T>(this T obj, Action<T> action) { action(obj); return obj; }
    public static T2 Do<T1, T2>(this T1 obj, Func<T1, T2> action) => action(obj);
    public static T1 DoInst<T1, T2>(this T1 obj, Func<T1, T2> action) { action(obj); return obj; }
}

public class SmartType
{
    private Type _type { get; set; }

    public SmartType(Type _type) => this._type = _type;

    public new Type GetType() { return _type; }
}

public class SmartType<T> : SmartType
{
    public SmartType() : base(typeof(T)) { }
}