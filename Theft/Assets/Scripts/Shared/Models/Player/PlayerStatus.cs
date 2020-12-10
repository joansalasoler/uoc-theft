using System;
using System.ComponentModel;
using UnityEngine;

namespace Game.Shared {

    /**
     * Encapsulates the current status of the player.
     */
    [CreateAssetMenu]
    public class PlayerStatus : ScriptableObject {

        /** Invoked when a property on this status changes */
        public Action<PlayerStatus> statusChanged;

        /** Number of books found */
        public int booksFound = 0;

        /** Current shield points */
        public int shieldPoints = 0;

        /** Current health points */
        public int healthPoints = 12;

        /** Bullets that can be shot */
        public int munitionLeft = 5;

        /** Books that can be collected */
        public int bookSlots = 5;

        /** Maximum number of shield points */
        public int shieldSlots = 4;

        /** Maximum number of health points */
        public int healthSlots = 12;

        /** Bullets that can be collected */
        public int munitionSlots = 15;


        /**
         * Check if the player has bullets.
         */
        public bool HasMunition() {
            return munitionLeft > 0;
        }


        /**
         * Reset this player status to defaults.
         */
        public void Reset() {
            booksFound = 0;
            shieldPoints = 0;
            healthPoints = 12;
            munitionLeft = 5;
            bookSlots = 5;
            shieldSlots = 4;
            healthSlots = 12;
            munitionSlots = 15;
            NotifyStatusChange();
        }


        /**
         * Refill the health reserve fully.
         */
        public bool RefillHealth() {
            bool canRefill = (healthPoints < healthSlots);

            if (canRefill == true) {
                healthPoints = healthSlots;
                NotifyStatusChange();
            }

            return canRefill;
        }


        /**
         * Refill the health by two units.
         */
        public bool IncreaseHealth() {
            bool canIncrease = (healthPoints < healthSlots);

            if (canIncrease == true) {
                healthPoints = Mathf.Min(healthPoints + 2, healthSlots);
                NotifyStatusChange();
            }

            return canIncrease;
        }


        /**
         * Refill the shield reserve fully.
         */
        public bool RefillShield() {
            bool canRefill = (shieldPoints < shieldSlots);

            if (canRefill == true) {
                shieldPoints = shieldSlots;
                NotifyStatusChange();
            }

            return canRefill;
        }


        /**
         * Refill the shield by one unit.
         */
        public bool IncreaseShield() {
            bool canIncrease = (shieldPoints < shieldSlots);

            if (canIncrease == true) {
                shieldPoints += 1;
                NotifyStatusChange();
            }

            return canIncrease;
        }


        /**
         * Increae the books found by one unit.
         */
        public bool IncreaseBooks() {
            bool canIncrease = (booksFound < bookSlots);

            if (canIncrease == true) {
                booksFound += 1;
                NotifyStatusChange();
            }

            return canIncrease;
        }


        /**
         * Refill the munition slots fully.
         */
        public bool RefillMunition() {
            bool canRefill = (munitionLeft < munitionSlots);

            if (canRefill == true) {
                munitionLeft = munitionSlots;
                NotifyStatusChange();
            }

            return canRefill;
        }


        /**
         * Decrease the left munition by one unit.
         */
        public bool DecreaseMunition() {
            bool canDecrease = (munitionLeft > 0);

            if (canDecrease == true) {
                munitionLeft -= 1;
                NotifyStatusChange();
            }

            return canDecrease;
        }


        /**
         * Refill the munition by one unit.
         */
        public bool InreaseMunition() {
            bool canIncrease = (munitionLeft < munitionSlots);

            if (canIncrease == true) {
                munitionLeft += 1;
                NotifyStatusChange();
            }

            return canIncrease;
        }


        /**
         * Decrease the player's shield and health.
         */
        public bool DamagePlayer() {
            if (shieldPoints < 1) {
                healthPoints -= 2;
                NotifyStatusChange();
            } else if (healthPoints > 0) {
                healthPoints -= 1;
                shieldPoints -= 1;
                NotifyStatusChange();
            }

            return healthPoints > 0;
        }


        /**
         * Invokes the statusChanged action.
         */
        private void NotifyStatusChange() {
            if (statusChanged != null) {
                statusChanged.Invoke(this);
            }
        }
    }
}
