;; Definitions for torpedo agents.

(ns pucks.agents.torpedo
  (:use [pucks globals util]
        [pucks.agents active zapper]))
  
(defn torpedo-proposals [p]
  {:rotation (direction->rotation (:velocity p))
   :transfer (into [] (for [victim (filter :mobile (:overlaps p))]
                        {:self (:id p)
                         :other (:id victim)
                         :bid {}
                         :ask {:energy 0.1}}))})

(defn torpedo []
  (merge (zapper)
         {:torpedo true
          :mobile true
          :proposal-function torpedo-proposals
          :color [200 100 0]
          :radius 8}))

