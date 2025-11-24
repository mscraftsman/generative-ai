namespace Mscc.GenerativeAI.Types
{
    public partial class MaskReferenceImage : ReferenceImage
    {
        public override ImageReferenceType ReferenceType => ImageReferenceType.ReferenceTypeMask;
    }
}