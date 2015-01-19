All pucks related programs run off Lee Spector’s Pucks environment: https://github.com/lspector/pucks. The also use the application Counterclockwise to run a Clojure REPL which launched a Java applet using Quil. The basic concept of pucks is creating intelligent and adaptable agents.

To run the pucks files, copy one of the user-puck files from the user-puck iterations folder into the user.clj file (located on the path pucks-master/src/pucks/agents/user.clj). Then, use the lean run pucks.worlds.ai.world1 command to launch the user puck in the world. Early puck iterations are built to function in world1. Later pucks, around iteration 0.06 are also built to function with beacons containing absolute world locations in their ids and can be launched in world 3 or 5. 

