(ns pucks.worlds.war.battle6
  (:use [pucks core globals]
        [pucks.agents stone vent zapper red-grunt red-gunner red-medic blue-grunt blue-gunner blue-medic nursery]))
(def size 1600)
(def centered (/ size 2))
(def red-x 150)
(def blue-x (- size 150))
(defn agents []
  (concat 
    ;stones
    (for [y (take (/ size 20) (iterate #(+ % 20) 0))]
     (merge (stone) {:position [(- size 25) y]}))
    (for [y (take (/ size 20) (iterate #(+ % 20) 0))]
      (merge (stone) {:position [25 y]}))
    
    ;red turrets
    [(merge (nursery red-gunner) {:position [350 (+ centered 500)]})]
    [(merge (nursery red-gunner) {:position [350 (- centered 500)]})]
    [(merge (nursery red-gunner) {:position [350 (+ centered 200)]})]
    [(merge (nursery red-gunner) {:position [350 (- centered 200)]})]
    
    ;red forces
     (for [y (take 5 (iterate #(+ % 30) centered))]
    (merge (red-grunt 1) {:position [red-x y]}))
  (for [y (take 5 (iterate #(+ % 30) centered))]
    (merge (red-medic 1) {:position [(+ 30 red-x) y]}))
  (for [y (take 5 (iterate #(+ % 30) centered))]
    (merge (red-grunt 2) {:position [(+ 60 red-x) y]}))
  (for [y (take 5 (iterate #(+ % 30) centered))]
    (merge (red-medic 2) {:position [(+ 90 red-x) y]}))
  (for [y (take 5 (iterate #(- % 30) centered))]
    (merge (red-medic 3) {:position [red-x y]}))
  (for [y (take 5 (iterate #(- % 30) centered))]
    (merge (red-medic 3) {:position [(+ 30 red-x) y]}))
  (for [y (take 5 (iterate #(- % 30) centered))]
    (merge (red-medic 4) {:position [(+ 60 red-x) y]}))
  (for [y (take 5 (iterate #(- % 30) centered))]
       (merge (red-medic 4) {:position [(+ 90 red-x) y]}))
    
    ;red team's vents
    [(merge (vent) {:position [250 (+ centered 500)]})]
    [(merge (vent) {:position [250 (- centered 500)]})]
    [(merge (vent) {:position [250 (+ centered 200)]})]
    [(merge (vent) {:position [250 (- centered 200)]})]
    
    
    ;creates the blue faction 
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 1) {:position [blue-x y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 2) {:position [(+ 30 blue-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 3) {:position [(+ 60 blue-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 4) {:position [(+ 90 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 5) {:position [blue-x y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 6) {:position [(+ 30 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 7) {:position [(+ 60 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
       (merge (blue-grunt 8) {:position [(+ 90 blue-x) y]}))
  ))
(defn settings []
  {
   :screen-size size
   :scale 0.5
   :single-thread-mode false
   })

;(run-pucks (agents) (settings))
