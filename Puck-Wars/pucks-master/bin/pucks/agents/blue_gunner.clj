(ns pucks.agents.blue-gunner
   (:use [pucks globals util vec2D maneuvers]
        [pucks.agents active]
        [clojure.set]))

(defn blue-gunner-proposals 
  [p]
  (let  
    [allies (concat (filter #(= (:mobile %) true) (filter :blue-grunt (:sensed p)))
                    (filter #(= (:mobile %) true) (filter :blue-medic (:sensed p))))]
  {
   :fire-torpedo (if (and (= (mod (:steps p) 5) 0)
                          (empty? allies))
                    true
                    false)
  }))
(defn blue-gunner []
  (merge (active)
         {:blue-gunner true
          :sensor-range 0
          :mobile false
          :radius 30
          :rotation (/ (* 3 pi) 2)
          :proposal-function blue-gunner-proposals
          :color [0 0 100]
          :eye-color [0 0 100]}))