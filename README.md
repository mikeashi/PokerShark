# PokerShark

PokerShark is An Open Source Texas no limit Hold'em player, based on HTN Planning techniques including risk-awareness.

## Goals

PokerShark is part of my bachelor thesis at university stuttgart. Project goals were described as follows:

> The game of poker is interesting in that it combines elements of stochasticity, reasoning, risk awareness, opponent modeling, and decision making with hidden information. The goal of this project is to study components that are necessary to build a successful AI poker player. Most notably:
>1) HTN Planning techniques that include risk-awareness
>2) Accurate opponent modeling based on game hand history
>3) Dynamic evaluation of the current state of the game based on hand history, stack values, and position
>4) Adaptation of strategies to type of game played (number of tables, speed, limits, etc.)
>
>The case study will be online No-Limit Hold 'em Multi-Table Tournaments.

## How does it work?
PokerShark is based on HTN Planning techniques including risk-awareness. In the preflop stage it uses expert knowledge to form a recommendation, which is then evaluated based on the current game situation to form a decision. Factors such as position, number of callers, number of raisers, opponents playing style, pot size ..etc are all taken into consideration when formaing the decision.

In the postflop stages of the game it uses an oracel to evaluate the strength of its hand. The hand evaluation is built using a combination of metrics such as hand strength (described in Aaron Davidson's masters thesis), hand potential, and Monte Carlo simulation.

PokerShark builds also a model for each opponent keeping track of thier action history, playing stats and playing style in addition to a weight table that describes the probability of the opponent holding every possible set of pocket cards. Using all of this information plus a dynamic risk attitude, PokerShark will choose the move with the best utility. 

<p align="center" width="100%">
    <img src="https://user-images.githubusercontent.com/25008083/198908427-9ab55251-21aa-4d3f-ad7e-721c41c27e03.png"> 
</p>

## Play against PokerShark

We have adopted the PyPoker project to work with PokerShark you can play using the web interface or using the console. We have provided easy to use docker image that can be started in matter of seconds. For more information please check [PokerServer](https://github.com/mikeashi/PokerServer).

<p align="center" width="100%">
    <img src="https://user-images.githubusercontent.com/25008083/198908733-0dd7e3ed-961d-4efd-89e0-0ddf9bc04c37.png"> 
</p>


Just make sure to start PokerShark and dont cheat using its informative console :) 

<p align="center" width="100%">
    <img src="https://user-images.githubusercontent.com/25008083/198908990-5e31c966-288b-4906-9657-4667620f61d3.png"> 
</p>

## Read The Thesis
will come soon x.0

## Acknowledgements
- [Pål Trefall](https://github.com/ptrefall) for the amazing HTN planner [fluid-hierarchical-task-network](https://github.com/ptrefall/fluid-hierarchical-task-network).
- [Keith Rule](https://www.codeproject.com/script/Membership/View.aspx?mid=120) for the great library [Holdem Hand](https://www.codeproject.com/Articles/12279/Fast-Texas-Holdem-Hand-Evaluation-and-Analysis).
- [@ishikota](https://github.com/ishikota) for the great projects [PyPokerEngine](https://github.com/ishikota/PyPokerEngine), [PyPokerGUI](https://github.com/ishikota/PyPokerGUI).
- [Alan Hemmings](https://github.com/goblinfactory) for the pretty console library [konsole](https://github.com/goblinfactory/konsole).

