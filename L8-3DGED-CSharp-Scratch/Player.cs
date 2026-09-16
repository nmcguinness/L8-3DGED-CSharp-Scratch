using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace L8_3DGED_CSharp_Scratch
{
    /// <summary>
    /// A simple class representing a player in a game, with properties for actor type, health, and position. Provides input validation for actor type and health, and a read-only property to determine if the player is alive.
    /// Demonstrates the use of a user-defined class (Vector3) as a property type, and the use of encapsulation to protect the internal state of the player object.
    /// </summary>
    public class Player
    {
        private string actorType;
        private int health;
        private Vector3 position;

        //private bool isAlive; //we can use health to determine if the player is alive or not, so we don't need this variable

        public string ActorType
        {
            get { return actorType; }
            set { actorType = value != null && value.Length != 0 ? value : "Default Type"; }
        }

        public int Health
        {
            get { return health; }
            set { health = value < 0 ? 0 : value; } //health cannot be negative
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsAlive
        {
            get { return health > 0; } //if health is greater than 0, the player is alive
        }

        public Player()
        {
            ActorType = "Default Type";
            Health = 100;
            Position = new Vector3();
        }

        public Player(string actorType, int health, Vector3 position)
        {
            ActorType = actorType;
            Health = health;
            Position = position;
        }

        //TODO - ToString, ShallowCopy, DeepCopy, Equals, GetHashCode, and any other methods you want to add to the Player class

        public override string ToString()
        {
            return $"Actor: {actorType} at {position}";
        }

        public Player ShallowCopy()
        {
            return this; 
        }
        public Player DeepCopy()
        {
            return new Player(actorType, health, position.DeepCopy());
        }

        public override bool Equals(object obj)
        {
            Player other = obj as Player; //if this fails then null will be returned, so we need to check for null
            //Player another = (Player)obj;  //if fails then runtime exception will be thrown

            if (other == null)
                return false;

           if (ReferenceEquals(this, other)) //two variables point to the same object in memory
                    return true;
            
            //you decide what properties you want to compare, in this case we will compare actorType, health, and position
            return this.actorType == other.actorType 
                && this.health == other.health 
                && this.position.Equals(other.position);
        }


    }
}
