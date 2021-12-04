using System;

namespace Utilities.Games.Models.Contracts.Attributes
{
    /// <summary>
    /// Attribute for defining the relationship between a lookup property and a foreign object.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PseudoForeignKeyAttribute : Attribute
    {
        readonly Type foreignType;
        /// <summary>
        /// Reference to the lookup property's target object.
        /// </summary>
        public Type ForeignType => foreignType;

        public string foreignKey;
        /// <summary>
        /// Reference to the name of the property on the target object for which to compare the lookup property value(s).
        /// </summary>
        public String ForeignKey => foreignKey;

        // This is a positional argument
        public PseudoForeignKeyAttribute(Type foreignType, string foreignKey)
        {
            this.foreignType = foreignType;
            this.foreignKey = foreignKey;
        }

    }
}
