using AutoMapper;
using System.Collections.Generic;

namespace AspNetScaffolding.Extensions.Mapper
{
    public static class GlobalMapper
    {
        public static IMapper Mapper { get; set; }

        public static TDestination Map<TDestination>(this object source) 
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map<TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source) 
            where TSource : class 
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map(source, destination);
        }

        public static IEnumerable<TDestination> MapCollection<TDestination>(this IEnumerable<object> source)
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map<IEnumerable<TDestination>>(source);
        }

        public static TDestination As<TDestination>(this object source)
           where TDestination : class
        {
            return GlobalMapper.Mapper.Map<TDestination>(source);
        }

        public static TDestination As<TSource, TDestination>(this TDestination destination, TSource source)
            where TSource : class
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map(source, destination);
        }

        public static IEnumerable<TDestination> AsCollection<TDestination>(this IEnumerable<object> source)
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map<IEnumerable<TDestination>>(source);  
        }
    }
}
