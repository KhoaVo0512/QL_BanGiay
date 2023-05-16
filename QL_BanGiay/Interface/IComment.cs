using QL_BanGiay.Data;

namespace QL_BanGiay.Interface
{
    public interface IComment
    {
        bool CreateComment(string maGiay, int stars, string comment, string name);
        int getStars(string shoeId);
        int getCommentLength(string shoeId);
        List<DanhGium> GetComment(string shoeId);
        int getPercent5Star(string shoeId);
        int getPercent4Star(string shoeId);
        int getPercent3Star(string shoeId);
        int getPercent2Star(string shoeId);

    }
}
