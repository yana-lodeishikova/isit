;;****************
;;* DEFFUNCTIONS *
;;****************

(deffunction ask-question (?question $?allowed-values)
  (printout t ?question)
  (bind ?answer (read))
  (if (lexemep ?answer)
    then (bind ?answer (lowcase ?answer))
  )
  (while (not (member$ ?answer ?allowed-values)) do
    (printout t ?question)
    (bind ?answer (read))
    (if (lexemep ?answer)
      then (bind ?answer (lowcase ?answer)))
  )
  ?answer
)

(deffunction ask-question-yes-no (?question)
  (bind ?response (ask-question ?question yes no y n))
  (if (or (eq ?response yes) (eq ?response y))
    then yes
    else no
  )
)

;;;***************
;;;* QUERY RULES *
;;;***************

(defrule determine-device ""
  (not (device ?))
  (not (result ?))
  =>
  (assert (device (ask-question
    "К какому устройству вы будете подключать наушники (phone/pc/tv)? "
    phone pc tv
  )))
)

(defrule determine-purpose ""
  (not (device ?))
  (not (result ?))
  =>
  (assert (purpose (ask-question
    "Для чего чаще всего будут использоваться наушники (calls/games/video/music)? "
    calls games video music
  )))
)

(defrule determine-budget ""
  (not (budget ?))
  (not (result ?))
  =>
  (assert (budget (ask-question
    "Какой у вас бюджет (1000/5000/10000)?"
    1000 5000 10000
  )))
)

;;;**********************
;;;* INTERMEDIATE RULES *
;;;**********************

;Определение подходящей конструкции, затычки или охватывающие
(defrule determine-form-over-ear ""
  (device phone)
  (not (form ?))
  (not (result ?))
  =>
  (assert (form in-ear))
)
(defrule determine-form-over-ear ""
  (or (device pc) (device tv))
  (not (form ?))
  (not (result ?))
  =>
  (assert (form over-ear))
)

;Определение способа подключения, проводные или беспроводные
(defrule determine-wireless-yes ""
  (or 
    (device tv)
    (and (device phone) (or (purpose calls) (purpose music)))
    (and (device pc) (or (purpose games) (purpose video) (purpose music)))
  )  
  (not (wireless ?))
  (not (result ?))
  =>
  (assert (wireless yes))
)
(defrule determine-wireless-no ""
  (or 
    (and (device phone) (or (purpose games) (purpose video)))
    (and (device pc) (purpose calls))
  )  
  (not (wireless ?))
  (not (result ?))
  =>
  (assert (wireless no))
)

;Определение, нужен микрофон или нет
(defrule determine-microphone-yes ""
  (or (purpose calls) (purpose games))
  (not (microphone ?))
  (not (result ?))
  =>
  (assert (microphone yes))
)
(defrule determine-microphone-no ""
  (or (purpose video) (purpose music))
  (not (microphone ?))
  (not (result ?))
  =>
  (assert (microphone no))
)

;;;****************
;;;* RESULT RULES *
;;;****************

(defrule no-result ""
  (declare (salience -10))
  (not (result ?))
  =>
  (assert (result "Невозможно найти подходящий вариант"))
)

(defrule result-cadena ""
  (form in-ear)
  (wireless no)
  (microphone yes)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "CADENA YH-12"))
)

(defrule result-jvc ""
  (form in-ear)
  (wireless no)
  (microphone no)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "JVC HA-FX38"))
)

(defrule result-qcy ""
  (form in-ear)
  (wireless yes)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "QCY T1c"))
)

(defrule result-sennheiser ""
  (form in-ear)
  (wireless no)
  (budget 5000)
  (not (result ?))
  =>
  (assert (result "Sennheiser CX 300S"))
)

(defrule result-jbl-1 ""
  (form in-ear)
  (wireless yes)
  (budget 5000)
  (not (result ?))
  =>
  (assert (result "JBL Wave 200TWS"))
)

(defrule result-samsung ""
  (form in-ear)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "Samsung Buds 2"))
)

(defrule result-a4tech ""
  (form over-ear)
  (wireless no)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "A4Tech HS-200"))
)

(defrule result-harper ""
  (form over-ear)
  (wireless yes)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "Harper HB-210"))
)

(defrule result-soundcore ""
  (form over-ear)
  (wireless yes)
  (microphone no)
  (budget 5000)
  (not (result ?))
  =>
  (assert (result "Soundcore Life 2 Neo Q10i"))
)

(defrule result-sven ""
  (form over-ear)
  (wireless yes)
  (microphone yes)
  (budget 5000)
  (not (result ?))
  =>
  (assert (result "SVEN AP-B900MV"))
)

(defrule result-steelseries ""
  (form over-ear)
  (wireless no)
  (budget 5000)
  (not (result ?))
  =>
  (assert (result "SteelSeries Arctis 1"))
)

(defrule result-hyperx ""
  (form over-ear)
  (wireless no)
  (microphone yes)
  (budget 10000)
  (not (result ?))
  =>
  (assert (result "HyperX Cloud Alpha S HX-HSCAS"))
)

(defrule result-jbl-2 ""
  (form over-ear)
  (wireless yes)
  (microphone yes)
  (budget 1000)
  (not (result ?))
  =>
  (assert (result "JBL Tune 710BT"))
)

(defrule result-fostex ""
  (form over-ear)
  (wireless no)
  (microphone no)
  (budget 10000)
  (not (result ?))
  =>
  (assert (result "Fostex T20RP MK3"))
)


;;;********************************
;;;* STARTUP AND CONCLUSION RULES *
;;;********************************

(defrule system-banner ""
  (declare (salience 10))
  =>
  (printout t crlf crlf)
  (printout t "Система рекомендации покупки наушников")
  (printout t crlf crlf)
)

(defrule print-result ""
  (declare (salience 10))
  (result ?item)
  =>
  (printout t crlf crlf)
  (printout t "Предлагаемая модель:")
  (printout t crlf crlf)
  (format t " %s%n%n%n" ?item)
)
