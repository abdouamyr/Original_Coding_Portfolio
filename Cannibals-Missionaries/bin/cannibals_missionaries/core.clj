(ns cannibals-missionaries.core)

;from Lee's Gist
(defn search
  [goal start combiner successors]
  (loop [frontier [(hash-map :contents start :history [])]
         seen #{start} ;;***
         steps 0]      ;;***
    (print frontier)
    (if (empty? frontier)
      false
      (let [f (first frontier)
            r (rest frontier)]
        (if (goal (:contents f)) 
          [f {:seen (count seen) :steps steps}]
          (let [unseen-successors (clojure.set/difference ;;***
                                    (set (successors (:contents f)))
                                    seen)]
            (recur 
              (combiner 
                (map #(hash-map :contents % 
                                :history (conj (:history f) (:contents f))) 
                     unseen-successors)                   ;;***
                r)
              (clojure.set/union seen unseen-successors)  ;;***
              (inc steps))))))))         

(frontiers [3 3 1])
(defn frontiers [current-pos]               
  (let [b (nth current-pos 2) ;the boat variable
        current-bank (vec (take 5 (repeat current-pos))) ;creates 5 copies of the vector passed to it
        pos-values (vec '([0 1 1] 
                           [1 0 1] 
                           [1 1 1] 
                           [2 0 1]
                           [0 2 1])) ;all the transformations stored in vectors  
        function (if (= b 1)
                   -
                   +) ;based on which side the boat is on, determines the operation to use
        ]
    (let [raw-frontier (mapv vec (partition 3 (vec (map function (vec (flatten current-bank)) (vec (flatten pos-values))))))] ;creates a vector of transformations of the passed vector      
      (loop [
             frontier raw-frontier ;variable for the vectors to search 
             keep-val [] ;the vectors that passed the conditions 
             ]
        (if (empty? frontier) 
             (mapv vec (partition 3 keep-val)) ;returns the vectors, if they were deemed acceptable
             (let [
                   c (nth (first frontier) 0)
                   m (nth (first frontier) 1)
                   b (nth (first frontier) 2)]
               (if (and (>= c 0) (>= m 0) (<= c 3)(<= m 3)(or (= m c) (= m 3)(= m 0))) ;checks each vector, bsaed on the parameters of the assignment
                 (recur (rest frontier)
                        (concat (first frontier) keep-val)) ;adds the vector if acceptable
                 (recur (rest frontier)
                        keep-val))))))))
               
(def wrong-bank [3 3 1])

(defn win-con
  [bank]
  (if (= (vec (flatten bank)) [0 0 0])
    true
    false))

(defn valid-moves
  [bank]
  (case bank
    [3 3 1] [[2 2 0]
             [1 3 0]
             [2 3 0]]
    [2 3 1] [[1 3 0]
             [2 2 0]
             [0 3 0]]
    [2 2 1] [[1 1 0]
             [2 0 0]]
    [1 1 1] [[0 0 0]
             [0 1 0]
             [1 0 0]]
    [1 0 1] [[0 0 0]]
    [1 3 1] [[1 1 0]]
    [3 0 1] [[1 0 0]
             [2 0 0]]
    [2 0 1] [[0 0 0]
             [1 0 0]]
    [2 3 0] [[3 3 1]]
    [2 2 0] [[2 3 1]
             [3 3 1]]
    [1 1 0] [[2 2 1]]
    [1 3 0] [[2 3 1]
             [3 3 1]]
    [0 3 0] [[1 3 1]
             [2 3 1]]
    [2 0 0] [[3 0 1]
             [2 2 1]]
    [1 0 0] [[2 0 1]
             [1 1 1]]
    ))

(defn bank-breadth 
  []
  (search  win-con
           [3 3 1] 
           #(concat %2 %1) 
           frontiers))
(bank-depth)
(bank-breadth)
(defn bank-depth 
  []
  (search  win-con
           [3 3 1] 
           concat 
           frontiers))