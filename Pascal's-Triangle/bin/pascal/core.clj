(ns pascal.core)

(defn factorial 
  [i]
  (apply * (map - (take i (repeat i)) (range i))))

(defn choose 
  [i x]
  (/ (factorial i) (* (factorial x) (factorial (- i x)))))

(defn pascal-line
  [i s]
  (cons 
    (cons 
      (take (/ s 1.75) (repeat " "))
      (map choose
           (take (+ i 1)(repeat i)) 
           (range (+ i 1))))
    "\n")
  )

(defn pascal 
  [i]
  (print
    (flatten
      (map pascal-line (range (+ i 1)) (map - (take i (repeat i)) (range i)))
      )))
