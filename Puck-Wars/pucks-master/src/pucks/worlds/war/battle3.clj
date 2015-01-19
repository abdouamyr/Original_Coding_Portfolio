;the battlefield with nothing
(ns pucks.worlds.war.battle3
  (:use [pucks core globals]
        [pucks.agents stone vent zapper red-grunt red-gunner blue-grunt blue-gunner nursery]))
(def size 1600)
(def centered (/ size 2))
(def red-x 150)
(def blue-x (- size 150))
(defn agents []
  (concat 
  ;creates the red faction
   (for [y (take 20 (iterate #(+ % 30) centered))]
     (merge (red-grunt 1) {:position [red-x y]}))
    (for [y (take 20 (iterate #(- % 30) centered))]
      (merge (red-grunt 2) {:position [red-x y]}))
      (for [y (take 5 (iterate #(+ % 100) centered))]
     (merge (vent) {:position [red-x y]}))
    (for [y (take 5 (iterate #(- % 100) centered))]
     (merge (vent) {:position [red-x y]}))
    
    ;creates the blue faction 
   (for [y (take 20 (iterate #(+ % 30) centered))]
     (merge (blue-grunt 1) {:position [blue-x y]}))
   (for [y (take 20 (iterate #(- % 30) centered))]
     (merge (blue-grunt 2) {:position [blue-x y]}))
   
   (for [y (take 5 (iterate #(+ % 100) centered))]
     (merge (vent) {:position [blue-x y]}))
   (for [y (take 5 (iterate #(- % 100) centered))]
     (merge (vent) {:position [blue-x y]}))
    ))

(defn settings []
  {
   :screen-size size
   :scale 0.5
   :single-thread-mode false
   })

;(run-pucks (agents) (settings))