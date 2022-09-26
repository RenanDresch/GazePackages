// namespace Gaze.MVVM.Services
// {
//     public static class ReactiveListExtensions
//     {
//         public static void Add<T>(this ReactiveList<T> reactiveList, T newItem)
//         {
//             reactiveList.Writer.Add(newItem);
//         }
//         
//         public static bool Remove<T>(this ReactiveList<T> reactiveList, T item)
//         {
//             return reactiveList.Writer.Remove(item);
//         }
//         
//         public static void Insert<T>(this ReactiveList<T> reactiveList, int index, T newItem)
//         {
//             reactiveList.Writer.Insert(index, newItem);
//         }
//         
//         public static void RemoveAt<T>(this ReactiveList<T> reactiveList, int index)
//         {
//             reactiveList.Writer.RemoveAt(index);
//         }
//         
//         public static void Clear<T>(this ReactiveList<T> reactiveList)
//         {
//             reactiveList.Writer.Clear();
//         }
//     }
// }