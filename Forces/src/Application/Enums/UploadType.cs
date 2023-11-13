using System.ComponentModel;

namespace Forces.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images\Products")]
        Product,

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document,
        [Description(@"Qoutaions")]
        Qoutaions,
        [Description(@"Qoutaions\Departments")]
        DepartmentQoutation
    }
}