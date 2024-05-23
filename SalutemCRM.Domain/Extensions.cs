using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SalutemCRM;

public delegate void SearchHandler(string keyword);

public static partial class Extensions
{
    //public static T? Cast<T>(this object obj) where T : class => obj as T;
    public static T? Cast<T>(this object obj) => (T)obj;
    public static T? EnumCast<T>(this object obj) => (T)Enum.Parse(typeof(T), obj.ToString()!);
    public static T? DoIf<T>(this T obj, Action<T> action, Func<T, bool> predicate) { if (predicate(obj)) { action(obj); return obj; } return default(T?); }
    public static T? Do<T>(this T obj, Func<T, bool> action) => action(obj) ? obj : default(T?);
    public static T Do<T>(this T obj, Action<T> action) { action(obj); return obj; }
    public static T2 Do<T1, T2>(this T1 obj, Func<T1, T2> action) => action(obj);
    public static T1 DoInst<T1, T2>(this T1 obj, Func<T1, T2> action) { action(obj); return obj; }

    public static IEnumerable<T> Move<T>(this ObservableCollection<T> list, T item, int newIndex) => list.ToList().Move(item, newIndex);

    public static IEnumerable<T> Move<T>(this List<T> list, T item, int newIndex)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1)
            {
                list.RemoveAt(oldIndex);

                if (newIndex > oldIndex) newIndex -= 1;

                list.Insert(newIndex, item);
            }
        }

        return list;
    }

    public static double PercentageCalc(double total, double val) => 100.0 / total * val;

    public static IEnumerable<T> DoForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
            action(item);
        return source;
    }
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