namespace MSSQLTEST.Models.Request
{
    public  class Creature
    {
        public string Blood { get; set; }
    }

    public interface IAttackable
    {
        public string attack(string test);


    }

    public class Monster : Creature, IAttackable
    {
        public string attack(string test)
        {
            throw new NotImplementedException();
        }
    }
    public class Player : Creature, IAttackable
    {
        public string attack(string test)
        {
            if (string.IsNullOrEmpty(test))
            {
                return ("aa");
            }
            else
            {
                return ("bb");
            }
        }


        IAttackable atk = new Monster();

    }
}
