using System;

namespace AspNetScaffolding.Utilities
{
    public static class ObjectMappingExtension
    {
        public static bool IsEligibleWheNotNull(this object srcMember, bool ignoreDefaultEnum = false)
        {
            if (srcMember == null)
            {
                return false;
            }

            if (ignoreDefaultEnum)
            {
                Type typeSrcMember = srcMember?.GetType();
                if (typeSrcMember.IsEnum && typeSrcMember.GetEnumValues().GetValue(0).ToString().ToLower() == srcMember.ToString().ToLower())
                {
                    return false;
                }
            }

            if (srcMember.ToString() == Guid.Empty.ToString())
            {
                return false;
            }

            return true;
        }
    }
}
