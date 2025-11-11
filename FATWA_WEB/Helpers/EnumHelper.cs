namespace FATWA_WEB.Helpers
{
    public static class EnumHelper
    {
        public static List<EnumInfo> GetEnumInfoList<T>() where T : Enum
        {
            var enumInfoList = new List<EnumInfo>();

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                enumInfoList.Add(new EnumInfo
                {
                    Id = (int)value,
                    Name = value.ToString(),
                });
            }

            return enumInfoList;
        }

        public class EnumInfo
        {
            public int Id;
            public string Name;
        }
    }
}
