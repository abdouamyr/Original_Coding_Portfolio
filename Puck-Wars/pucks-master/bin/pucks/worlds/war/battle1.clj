;a pucks battlefield
(ns battle1  
  (:use [pucks core globals]
        [pucks.agents stone vent zapper red-grunt red-gunner blue-grunt blue-gunner nursery]))
(defn agents []
  (concat 
    ;creates the stones that provide cover 
    (for [y (take 20 (iterate #(+ % 20) 100))]
      (merge (stone) {:position [500 y]}))
    (for [y (take 20 (iterate #(+ % 20) 100))]
     (merge (stone) {:position [300 y]}))
    
    ;creates the stones that fence off the back of the world so that pucks can't be outflanked
    (for [y (take 40 (iterate #(+ % 20) 0))]
      (merge (stone) {:position [775 y]}))
    (for [y (take 40 (iterate #(+ % 20) 0))]
      (merge (stone) {:position [25 y]}))
    
    ;creates the vents on the red side 
   [(merge (vent) {:position [50 300]})]
     [(merge (vent) {:position [50 650]})]
   [(merge (vent) {:position [50 50]})]
   
   ;creates the vents on the blue side 
   [(merge (vent) {:position [750 300]})]
   [(merge (vent) {:position [750 650]})]
   [(merge (vent) {:position [750 50]})]
   
   ;creates the vents in the middle of the field
    [(merge (vent) {:position [400 50]})]
   [(merge (vent) {:position [400 650]})]
   
    ;creates the red faction
   (for [y (take 10 (iterate #(+ % 30) 100))]
     (merge (red-grunt 1) {:position [100 y]}))
   [(merge (nursery red-gunner) {:position [50 700]})]
   [(merge (nursery red-gunner) {:position [50 600]})]
   
   ;creates the blue faction 
   (for [y (take 10 (iterate #(+ % 30) 100))]
     (merge (blue-grunt 1) {:position [700 y]}))
   [(merge (nursery blue-gunner) {:position [725 700]})]
   [(merge (nursery blue-gunner){:position [725 600]})]
   ))
   

(defn settings []
  {
   :single-thread-mode false
   })

(run-pucks (agents) (settings))