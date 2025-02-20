using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The Schema object allows the definition of input and output data types. These types can be objects, but also primitives and arrays. Represents a select subset of an OpenAPI 3.0 schema object.
    /// </summary>
    public class Schema
    {
        /// <summary>
        /// Required. Data type.
        /// </summary>
        public ParameterType? Type { get; set; }
        /// <summary>
        /// Optional. The format of the data.
        /// This is used only for primitive datatypes.
        /// Supported formats:
        ///  for NUMBER type: float, double
        ///  for INTEGER type: int32, int64
        ///  for STRING type: enum, date-time
        /// </summary>
        public string Format { get; set; } = "";
        /// <summary>
        /// Optional. A brief description of the parameter. This could contain examples of use. Parameter description may be formatted as Markdown.
        /// </summary>
        public string Description { get; set; } = "";
        /// <summary>
        /// Optional. Indicates if the value may be null.
        /// </summary>
        public bool? Nullable { get; set; }
        /// <summary>
        /// Optional. Schema of the elements of Type.ARRAY.
        /// </summary>
        public Schema? Items { get; set; }
        /// <summary>
        /// Optional. Maximum number of the elements for Type.ARRAY.
        /// </summary>
        public long? MaxItems { get; set; }
        /// <summary>
        /// Optional. Minimum number of the elements for Type.ARRAY.
        /// </summary>
        public long? MinItems { get; set; }
        /// <summary>
        /// Optional. Possible values of the element of Type.STRING with enum format.
        /// For example we can define an Enum Direction as :
        /// {type:STRING, format:enum, enum:["EAST", NORTH", "SOUTH", "WEST"]}
        /// </summary>
        public List<string>? Enum { get; set; }
        /// <summary>
        /// Optional. Properties of Type.OBJECT.
        /// An object containing a list of "key": value pairs. Example: { "name": "wrench", "mass": "1.3kg", "count": "3" }.
        /// </summary>
        public dynamic? Properties { get; set; }
        /// <summary>
        /// Optional. The order of the properties. Not a standard field in open api spec. Used to determine the order of the properties in the response.
        /// </summary>
        public List<string>? PropertyOrdering { get; set; }
        /// <summary>
        /// Optional. Required properties of Type.OBJECT.
        /// </summary>
        public List<string>? Required { get; set; }
    }
}
