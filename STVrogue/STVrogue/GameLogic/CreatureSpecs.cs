﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace STVrogue.GameLogic {

    public class CreatureSpecs {

        static public Boolean Monster() {
            return false;
        }

        static public Boolean MonsterAttackSpec(Game G, Creature foe) {
            // Initialize variables to work with.
            Creature creature = G.whoHasTheTurn;
            Monster monster = creature as Monster;
            Player player = G.player;
            int playerPreHP = player.HP;
            int damage = monster.attackRating;
            bool HPCorrectlyUpdated = false;
            // Invoke Monster.Attack().
            monster.Attack(G, foe);
            // Check if HP of foe was correctly updated.
            if ((playerPreHP - damage) == player.HP)
                HPCorrectlyUpdated = true;
            // Check if player's HP was correctly updated.
            return HPCorrectlyUpdated;

        }

        static public Boolean MonsterMoveSpec(Game G, Node n) {
            // Initialize variables to work with.
            Creature creature = G.whoHasTheTurn;
            Monster monster = creature as Monster;
            Node currentNode = monster.location;
            Zone currentZone = monster.location.zone;
            // Invoke Monster.Move().
            bool success = monster.Move(G, n);
            // Check if Monster.Move() returned false when the node the monster wanted to move to 
            // was not a neigbouring node of the monster's original location.
            if (!currentNode.neighbors.Contains(n))
                return success == false;
            // Check if Monster.Move() returned false when the zone the monster wanted to move to 
            // was not the same zone as the monster's original zone.
            if (!(currentZone == monster.location.zone))
                return success == false;
            // Check if Monster.Move() returned true and the monster's location was correctly updated.
            return success && monster.location == n;
        }

        static public Boolean MonsterFleeSpec(Game G, Node n) {
            // Initialize variables to work with.
            Creature creature = G.whoHasTheTurn;
            Monster monster = creature as Monster;
            Node currentNode = monster.location;
            Zone currentZone = monster.location.zone;
            Node playerLocation = G.player.location;
            // Invoke Monster.Flee().
            bool success = monster.Flee(G, n);
            // Check if Monster.Flee() returned false when the node the monster wanted to move to 
            // was not a neigbouring node of the monster's original location.
            if (!currentNode.neighbors.Contains(n))
                return success == false;
            // Check if Monster.Flee() returned false when the zone the monster wanted to move to 
            // was not the same zone as the monster's original zone.
            if (!(currentZone == monster.location.zone))
                return success == false;
            // Check if Monster.Flee() returned false when the node the monster wanted to move to 
            // was already occupied by the player.
            if (n == playerLocation)
                return success == false;
            // Check if Monster.Flee() returned true and the monster's location was correctly updated.
            return success && monster.location == n;
        }

        static public Boolean Player() {
            return false;
        }
        
        static public Boolean PlayerAttackSpec(Game G, Creature foe) {
            // Initialize variables to work with.
            Monster enemy = foe as Monster;
            Player player = G.player;
            int foePreHP = foe.HP;
            int damage = player.attackRating;
            bool isBoosted = player.boosted;
            bool HPCorrectlyUpdated = false;
            bool monsterDefeated = false;
            bool monsterDeleted = true;
            // Invoke Player.Attack().
            player.Attack(G, foe);
            // Check if HP of foe was correctly updated.
            if (isBoosted) {
                if ((foePreHP - 2 * damage) == foe.HP)
                    HPCorrectlyUpdated = true;
            } else {
                if ((foePreHP - damage) == foe.HP)
                    HPCorrectlyUpdated = true;
            }
            // Check if foe was correctly removed from Game's monster list if
            // it was killed.
            if (foe.HP <= 0) {
                monsterDefeated = true;
                monsterDeleted = false;
            }
            if (monsterDefeated && !G.monsters.Contains(enemy))
                monsterDeleted = true;
            // Check if defeated monster was correctly deleted and HP of foe was
            // correctly updated.
            return monsterDeleted && HPCorrectlyUpdated;
        }

        static public Boolean PlayerMoveSpec(Game G, Node n) {
            // Initialize variables to work with.
            Player player = G.player;
            Node currentNode = player.location;
            Item[] itemsInN = new Item[n.items.Count];
            n.items.CopyTo(itemsInN);
            // Invoke Player.Move().
            Boolean success = player.Move(G, n);
            // Check if all the items that were in the node have been transferred
            // to the players's bag.
            bool itemsInBag = false;
            foreach (Item i in itemsInN) {
                if (player.bag.Contains(i))
                    itemsInBag = true;
                else { 
                    itemsInBag = false;
                    break;
                }
            }
            // Check if 'inCombat' was set to true if node 'n' contained a monster.
            bool inCombatSet = false;
            bool containsMonster = false;
            bool inCombatCorrectlySet = false;
            List<Creature> nodeCreatures = n.monsters;
            if (!nodeCreatures.Any())
                containsMonster = true;
            if (player.inCombat == true)
                inCombatSet = true;
            if (containsMonster == inCombatSet)
                inCombatCorrectlySet = true;
            // Check if Player.Move() returned false when the node the player wanted to move to 
            // was not a neigbouring node of the player's original location.
            if (!currentNode.neighbors.Contains(n))
                return success == false;
            // Check if Player.Move() returned true, the player's location was updated, the node doesn't
            // contain any more items, all the items from the node were successfully tranferred to the bag
            // and we set the 'inCombat' variable to true when the node the player moved to contained a monster.
            return success && player.location == n && n.items.Count == 0 && itemsInBag && inCombatCorrectlySet;
        }

        static public Boolean PlayerFleeSpec(Game G, Node n) {
            Player player = G.player;
            Node currentNode = player.location;
            Item[] itemsInN = new Item[n.items.Count];
            n.items.CopyTo(itemsInN);
            // Invoke Player.Flee().
            Boolean success = player.Flee(G, n);
            // Check if all the items that were in the node have been transferred
            // to the players's bag.
            bool itemsInBag = false;
            foreach (Item i in itemsInN) {
                if (player.bag.Contains(i))
                    itemsInBag = true;
                else {
                    itemsInBag = false;
                    break;
                }
            }
            // Check if Player.Flee() returned false when the node the player wanted to move to 
            // was not a neigbouring node of the player's original location.
            if (!currentNode.neighbors.Contains(n))
                return success == false;
            // Check if Player.Flee() returned false when the node the player wanted to move to
            // contained a monster.
            bool containsMonster = false;
            List<Creature> nodeCreatures = n.monsters;
            if (!nodeCreatures.Any())
                containsMonster = true;
            if (containsMonster)
                return success == false;
            // Check if Player.Flee() returned true, the player's location was updated, the node doesn't
            // contain any more items and all the items from the node were successfully tranferred to the bag.
            return success && player.location == n && n.items.Count == 0 && itemsInBag;
        }
    }
}
