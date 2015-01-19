;the battlefield with nothing
(ns pucks.worlds.war.battle4
  (:use [pucks core globals]
        [pucks.agents stone vent zapper blue-grunt red-gunner red-grunt blue-gunner nursery]))
(def size 1600)
(def centered (/ size 2))
(def red-x 150)
(def blue-x (- size 150))
(defn agents []
  (concat 
  ;creates the red faction
  
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 1) {:position [red-x y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 2) {:position [(+ 30 red-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 3) {:position [(+ 60 red-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (blue-grunt 4) {:position [(+ 90 red-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 5) {:position [red-x y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 6) {:position [(+ 30 red-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 7) {:position [(+ 60 red-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (blue-grunt 8) {:position [(+ 90 red-x) y]}))
  
  (for [y (take 5 (iterate #(+ % 100) centered))]
    (merge (vent) {:position [red-x y]}))
  (for [y (take 5 (iterate #(- % 100) centered))]
    (merge (vent) {:position [red-x y]}))
  
  ;creates the blue faction 
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (red-grunt 1) {:position [blue-x y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (red-grunt 2) {:position [(+ 30 blue-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (red-grunt 3) {:position [(+ 60 blue-x) y]}))
  (for [y (take 20 (iterate #(+ % 30) centered))]
    (merge (red-grunt 4) {:position [(+ 90 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (red-grunt 5) {:position [blue-x y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (red-grunt 6) {:position [(+ 30 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
    (merge (red-grunt 7) {:position [(+ 60 blue-x) y]}))
  (for [y (take 20 (iterate #(- % 30) centered))]
       (merge (red-grunt 8) {:position [(+ 90 blue-x) y]}))
  
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
