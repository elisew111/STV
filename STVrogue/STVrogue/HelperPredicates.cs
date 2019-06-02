using System;
using System.Collections.Generic;
using STVrogue.GameLogic;

namespace STVrogue {
    /* 
     * Contain the forall and exists quantifiers, and some additional predicate
     * operators.   
     * Also contains a bunch of useful helper predicates. 
     */
    public class HelperPredicates {
        /*
         * Forall- and exists- quantifiers over a collections (you can also use them
         * to quantify over arrays). 
         * Example: forall(C, x => x>=0) checks whether all integers in the collection 
         * C are non-negative.
         */
        static public Boolean forall<T>(ICollection<T> C, Predicate<T> p) {
            foreach (T x in C)
                if (!p(x)) return false;
            return true;
        }

        /*
         * Exist-quantifier over a collection. Example: forall(C, x => x<0) checks whether 
         * the collection C contains a negative integer.
         */
        static public Boolean exists<T>(ICollection<T> C, Predicate<T> p) {
            return !forall(C, x => !p(x));
        }

        // just for demonstrating the syntax to you:
        private void test() {
            List<int> C = new List<int>();
            forall(C, x => x >= 0);
            forall(C, (int x) => x >= 0); // if you need to explicitly specify the type of x
            exists(C, x => x < 0);
            exists(C, (int x) => x < 0);
        }

        /* Check if p implies q (which is equivalent to !p or q). */
        static public Boolean imp(Boolean p, Boolean q) {
            return !p || q;
        }


        /** Check if a zone contains exactly one start-node. */
        static public Boolean hasOneStartZone(Zone zone) {
            int count = 0;
            foreach (Node x in zone.getNodes()) {
                if (x.type == NodeType.STARTnode) count++;
            }
            return count == 1;
        }

        /** Check if a zone contains exactly one exit-node. */
        static public Boolean hasOneExitZone(Zone zone) {
            int count = 0;
            foreach (Node x in zone.getNodes())
                if (x.type == NodeType.EXITnode)
                    count++;
            return count == 1;
        }


        /** Check if the zone is indeed fully connected. */
        static public Boolean isConnected(Zone zone) {
            bool result = true;
            foreach (Node node1 in zone.getNodes()) {
                foreach (Node node2 in zone.getNodes()) {
                    if (!node1.isReachable(node2))
                        result = false;
                }
            }
            return result;
        }


    }


}