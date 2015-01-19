(ns pucks.agents.red-gunner
   (:use [pucks globals util vec2D maneuvers]
        [pucks.agents active]
        [clojure.set]))

(defn red-gunner-proposals
  [p]
  (let  
    [allies (concat (filter #(= (:mobile %) true) (filter :red-grunt (:sensed p)))
                    (filter #(= (:mobile %) true) (filter :red-medic (:sensed p))))]
  {
   :fire-torpedo (if (and (= (mod (:steps p) 5) 0)
                          (empty? allies))
                    true
                    false)
   }))
(defn red-gunner []
  (merge (active)
         {:red-gunner true
          :sensor-range 0
          :mobile false
          :radius 30
          :rotation (/  pi 2)
          :proposal-function red-gunner-proposals
          :color [100 0 0]
          :eye-color [100 0 0]}))