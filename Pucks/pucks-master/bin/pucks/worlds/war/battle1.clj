;a pucks battlefield
(ns battle1  
  (:use [pucks core globals]
        [pucks.agents stone vent zapper red-grunt blue-grunt]))

(defn agents []
  (concat 
    (for [y (take 20 (iterate #(+ % 20) 100))]
      (merge (stone) {:position [500 y]}))
    (for [y (take 40 (iterate #(+ % 20) 0))]
      (merge (stone) {:position [775 y]}))
   (for [y (take 20 (iterate #(+ % 20) 100))]
     (merge (stone) {:position [300 y]}))
    (for [y (take 40 (iterate #(+ % 20) 0))]
     (merge (stone) {:position [25 y]}))
   [(merge (vent) {:position [50 300]})]
    [(merge (vent) {:position [750 300]})]
     [(merge (vent) {:position [400 50]})]
      [(merge (vent) {:position [400 650]})]
        [(merge (vent) {:position [50 650]})]
          [(merge (vent) {:position [50 50]})]
            [(merge (vent) {:position [750 650]})]
              [(merge (vent) {:position [750 50]})]
    ;           (merge (zapper) {:position [300 300]})
    (for [y (take 10 (iterate #(+ % 30) 100))]
      (merge (blue-grunt) {:position [700 y]}))
    (for [y (take 10 (iterate #(+ % 30) 100))]
      (merge (red-grunt) {:position [100 y]}))))

(defn settings []
  {})

(run-pucks (agents) (settings))