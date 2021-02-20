using Microsoft.Xna.Framework;

namespace GrimGame.Engine.AI
{
    public class FuzzyState
    {
        public struct Hp
        {
            public int            Amount;
            public FuzzyVariables Degree;
        }
        public struct EnemyHp
        {
            public int            Amount;
            public FuzzyVariables Degree;
        }
        public struct InflictedDamage
        {
            public int            Amount;
            public FuzzyVariables Degree;
        }
        public struct ReceivedDamage
        {
            public int            Amount;
            public FuzzyVariables Degree;
        }

        public struct Distance
        {
            public Vector2        distance;
            public FuzzyVariables Degree;
        }
        public struct Energy
        {
            public float          Amount;
            public FuzzyVariables Degree;
        }
        
        public enum Input { Hp = 0, EnemyHp = 0, InflictedDamage = 0, ReceivedDamage = 0, Distance = 0, Energy = 0 }

        public Input input;
        public enum Output { DoDamage, Rdd, Heal, Run, DoNothing }

        public Output output;
        public enum FuzzyVariables { Low, Mid, Hurt }
    }
}