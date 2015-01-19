;a large pucks battlefield
(ns battle2  
  (:use [pucks core globals]
        [pucks.agents stone vent zapper red-grunt red-gunner red-medic blue-grunt blue-gunner blue-medic nursery]))
(def size 1600)
(def centered (/ size 2))
(def red-x 150)
(def blue-x (- size 150))
(defn agents []
  (concat 
    ;creates the stones that provide cover 
    (for [y (take 20 (iterate #(+ % 20) centered))]
      (merge (stone) {:position [(- size 300) y]}))
    (for [y (take 20 (iterate #(- % 20) centered))]
      (merge (stone) {:position [(- size 300) y]}))
   (for [y (take 20 (iterate #(+ % 20) centered))]
      (merge (stone) {:position [300 y]}))
    (for [y (take 20 (iterate #(- % 20) centered))]
      (merge (stone) {:position [300 y]}))
    
    ;creates the stones that fence off the back of the world so that pucks can't be outflanked
    (for [y (take (/ size 20) (iterate #(+ % 20) 0))]
      (merge (stone) {:position [(- size 25) y]}))
    (for [y (take (/ size 20) (iterate #(+ % 20) 0))]
      (merge (stone) {:position [25 y]}))
    
    ;creates the vent on the red side 
   [(merge (vent) {:position [150 centered]})]
  
   
   ;creates the vent on the blue side 
   [(merge (vent) {:position [(- size 150) centered]})]

   
   ;creates the vents in the middle of the field
    [(merge (vent) {:position [(+ centered 250) (+ centered 250)]})]
    [(merge (vent) {:position [(- centered 250) (+ centered 250)]})]
    [(merge (vent) {:position [(+ centered 250) (- centered 250)]})]
    [(merge (vent) {:position [(- centered 250) (- centered 250)]})]
    [(merge (vent) {:position [centered 150]})]
    [(merge (vent) {:position [centered centered]})]
    [(merge (vent) {:position [centered (- size 150)]})]
   
    ;creates the red faction
   (for [y (take 10 (iterate #(+ % 30) centered))]
     (merge (red-grunt 1) {:position [red-x y]}))
   (for [y (take 10 (iterate #(+ % 30) centered))]
     (merge (red-medic 1) {:position [(- red-x 50) y]}))
    (for [y (take 10 (iterate #(- % 30) centered))]
     (merge (red-grunt 2) {:position [red-x y]}))
    (for [y (take 10 (iterate #(- % 30) centered))]
     (merge (red-medic 2) {:position [(- red-x 50) y]}))
   [(merge (nursery red-gunner) {:position [150 (+ centered 500)]})]
   [(merge (nursery red-gunner) {:position [150 (- centered 500)]})]
   
   ;creates the blue faction 
   (for [y (take 10 (iterate #(+ % 30) centered))]
     (merge (blue-grunt 1) {:position [blue-x y]}))
   (for [y (take 10 (iterate #(+ % 30) centered))]
     (merge (blue-medic 1) {:position [(+ blue-x 50) y]}))
   (for [y (take 10 (iterate #(- % 30) centered))]
     (merge (blue-grunt 2) {:position [blue-x y]}))
   (for [y (take 10 (iterate #(- % 30) centered))]
     (merge (blue-medic 2) {:position [(+ blue-x 50) y]}))
   [(merge (nursery blue-gunner) {:position [(- size 150) (+ centered 500)]})]
   [(merge (nursery blue-gunner){:position [(- size 150) (- centered 500)]})]
   ))
   

(defn settings []
  {
   :screen-size size
   :scale 0.5
   :single-thread-mode false
   })

(run-pucks (agents) (settings))