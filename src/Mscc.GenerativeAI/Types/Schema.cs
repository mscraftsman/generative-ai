using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class Schema
    {
        /// <summary>
        /// Optional. The type of the data.
        /// </summary>
        public ParameterType? Type { get; set; }
        /// <summary>
        /// Optional. The format of the data.
        /// Supported formats:
        ///  for NUMBER type: float, double
        ///  for INTEGER type: int32, int64
        /// </summary>
        public string Format { get; set; } = default;
        /// <summary>
        /// Optional. The description of the data.
        /// </summary>
        public string Description { get; set; } = default;
        /// <summary>
        /// Optional. Indicates if the value may be null.
        /// </summary>
        public bool? Nullable { get; set; }
        /// <summary>
        /// Optional. Schema of the elements of Type.ARRAY.
        /// </summary>
        public Schema? Items { get; set; }
        /// <summary>
        /// Optional. Possible values of the element of Type.STRING with enum format.
        /// For example we can define an Enum Direction as :
        /// {type:STRING, format:enum, enum:["EAST", NORTH", "SOUTH", "WEST"]}
        /// </summary>
        public List<string>? Enum { get; set; }
        /// <summary>
        /// Optional. Properties of Type.OBJECT.
        /// </summary>
        public dynamic? Properties { get; set; }
        /// <summary>
        /// Optional. Required properties of Type.OBJECT.
        /// </summary>
        public List<string>? Required { get; set; }
    }
}
