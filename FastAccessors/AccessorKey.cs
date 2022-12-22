namespace FastAccessors {
    using System;

    static class AccessorKey {
        const int prime = 16777619;
        internal static int Make(Type type, string fieldName) {
            unchecked {
                return type.GetHashCode() * prime + fieldName.GetHashCode();
            }
        }
        internal static string GetKey(Type type, string fieldName) {
            return GetTypeName(type) + "." + fieldName;
        }
        static string GetTypeName(Type type) {
            if(!type.IsGenericType)
                return type.Name;
            var sb = new System.Text.StringBuilder(type.Name);
            int argumentsPos = type.Name.IndexOf('`');
            sb.Remove(argumentsPos, type.Name.Length - argumentsPos);
            sb.Append('<');
            var genericArgs = type.GetGenericArguments();
            for(int i = 0; i < genericArgs.Length; i++) {
                sb.Append(GetTypeName(genericArgs[i]));
                if(i > 0) sb.Append(',');
            }
            sb.Append('>');
            return sb.ToString();
        }
    }
}