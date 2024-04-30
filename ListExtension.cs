using System.Collections.Generic;

namespace Publishing_house
{
    // Расширения для списка
    public static class ListExtension
    {
        public static bool EditById(this List<Composition> data, Composition d)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Id == d.Id)
                {
                    data[i] = d;
                    return true;
                }
            }

            return false;
        }

        public static bool RemoveById(this List<Composition> data, int id)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Id == id)
                {
                    data.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
