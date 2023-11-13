using Forces.Application.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum RankType
    {
        [imagePath("00")] [ArDescription("بدون رتبه")] [EnDescription("No Rank")] None = 0,
        [imagePath("O1")][ArDescription("ملازم")] [EnDescription("Second Lieutenant")] SecondLieutenant = 1,
        [imagePath("O2")][ArDescription("ملازم أول")] [EnDescription("First Lieutenant")] FirstLieutenan = 2,
        [imagePath("O3")][ArDescription("نقيب")] [EnDescription("Captain")] Captain = 3,
        [imagePath("O4")][ArDescription("رائد")] [EnDescription("Major")] Major = 4,
        [imagePath("O5")][ArDescription("مقدم")] [EnDescription("Lieutenant Colonel")] LieutenantColonel = 5,
        [imagePath("O6")][ArDescription("عقيد")] [EnDescription("Colonel")] Colonel = 6,
        [imagePath("O7")][ArDescription("عميد")] [EnDescription("Brigadier General")] BrigadierGeneral = 7,
        [imagePath("O8")][ArDescription("لواء")] [EnDescription("Major General")] MajorGeneral = 8,
        [imagePath("O9")][ArDescription("فريق")] [EnDescription("Lieutenant General")] LieutenantGeneral = 9,
        [imagePath("O10")][ArDescription("فريق أول")] [EnDescription("General")] General = 10,
        [imagePath("S7")][ArDescription("وكيل أول")] [EnDescription("WKL")] WKL1 = 11,
        [imagePath("S6")][ArDescription("وكيل ثاني")] [EnDescription("WKL/2")] WKL2 = 12,
        [imagePath("S5")][ArDescription("رقيب أول")] [EnDescription("RQB/1")] RQB1 = 13,
        [imagePath("S3")][ArDescription("رقيب")] [EnDescription("RQB")] RQB = 14,
        [imagePath("S2")][ArDescription("عريف")] [EnDescription("ARF")] ARF = 15,
        [imagePath("S1")][ArDescription("نائب عريف")] [EnDescription("N/R")] NR = 16,
        [imagePath("00")][ArDescription("جندي")] [EnDescription("JUNDI")] JUNDI = 17,


    }
}
