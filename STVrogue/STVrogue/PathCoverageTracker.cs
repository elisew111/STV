using System;
using System.Collections.Generic;
using System.Text;

namespace STVrogue {
    /*
     * A utility class to help you track path-coverage. Use addTargetPath to specify
     * the paths that we want to cover (target paths).
     * 
     *    - Use startPath() to signal the start of a test execution.
     *    - Use tickNode(nodeName) to signal visit to a node/state.
     *    - Use endPath() to signal the end of the test execution.
     *    - Use printSummary() etc to get information of coverage.
     */
    public class PathCoverageTracker {
        List<String> testRerquirements = new List<String>();
        List<String> executed = new List<String>();
        String currentPath = null;

        public PathCoverageTracker() { }

        /*
         * Add a path to cover. Specify it as a string in the format of node-ids separated by ":".
         * Example: "s1:s2:s3"
         */
        public void addTargetPath(String path) { testRerquirements.Add(path); }

        /* Start tracking a path. */
        public void startPath() { currentPath = ""; }

        /* Register that we pass the given node */
        public void tickNode(String node) {
            if (currentPath == "") currentPath += node;
            else currentPath += ":" + node;
        }

        /* Signal the end of the current path. It will then be added to the set of executed path. */
        public void endPath() {
            foreach (String p in executed) {
                if (p.Equals(currentPath)) break;
            }
            executed.Add(currentPath);
        }

        /* Return the list of covered targets. */
        public List<String> getCoveredPaths() {
            List<String> covered = new List<String>();
            foreach (String target in testRerquirements) {
                foreach (String sigma in executed) {
                    if (tour(sigma, target)) {
                        covered.Add(target); break;
                    }
                }
            }
            return covered;
        }

        /* Return the list of still uncovered targets. */
        public List<String> getUncoveredPaths() {
            List<String> covered = getCoveredPaths();
            List<String> uncovered = new List<String>();
            foreach (String p in testRerquirements) {
                Boolean cov = false;
                foreach (String pcov in covered) {
                    if (p.Equals(pcov)) {
                        cov = true; break;
                    }
                }
                if (!cov) uncovered.Add(p);
            }
            return uncovered;
        }

        /* Return the list of paths to cover. */
        public List<String> getTestRerquirements() {
            return testRerquirements;
        }

        /* Return the current set of test paths. */
        public List<String> getTestPaths() {
            return executed;
        }

        /* Check if path1 tours path2 */
        static public Boolean tour(String path1, String path2) {
            return path1.Contains(path2);
        }

        public String printCovered() {
            StringBuilder sb = new StringBuilder();
            List<String> covered = getCoveredPaths();
            for (int k = 0; k < covered.Count; k++) {
                sb.Append(covered[k]);
                sb.Append("\n");
            }
            sb.Append("Covered: " + covered.Count + " paths.");
            return sb.ToString();
        }

        public String printUncovered() {
            StringBuilder sb = new StringBuilder();
            List<String> uncovered = getUncoveredPaths();
            for (int k = 0; k < uncovered.Count; k++) {
                sb.Append(uncovered[k]);
                sb.Append("\n");
            }
            sb.Append("Uncovered: " + uncovered.Count + " paths.");
            return sb.ToString();
        }

        /* Printing a coverage report. */
        public String printSummary() {
            int N = testRerquirements.Count;
            int n = getCoveredPaths().Count;
            String z = "** The tests cover " + n + " targets out of " + N;
            if (n >= N) z += ". Well done!";
            else {
                z += "\n** Covered:";
                foreach (String s in getCoveredPaths()) {
                    z += "\n     " + s;
                }
                z += "\n** Uncovered:";
                foreach (String s in getUncoveredPaths()) {
                    z += "\n     " + s;
                }
            }
            return z;
        }


    }
}
