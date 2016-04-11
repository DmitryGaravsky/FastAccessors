namespace FastAccessors {
    static class AccessorKey {
        const int prime = 16777619;
        internal static int Make(System.Type type, string fieldName) {
            unchecked {
                return type.GetHashCode() * prime + fieldName.GetHashCode();
            }
        }
    }
}