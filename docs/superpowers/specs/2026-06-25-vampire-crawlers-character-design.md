# Vampire Crawlers Character Mod Design

Date: 2026-06-25

## Goal

Build a full playable Slay the Spire 2 character inspired by Vampire Crawlers. The first version should feel like a real character rather than a small prototype: complete starter deck, about 60-75 cards, 8-12 character relics, and a clear build identity.

The mod is a fan promotional project for Vampire Crawlers. The first version may use Vampire Crawlers-style names, references, and placeholder art to save time. If the mod is later prepared for broad public release, add a clear non-official fan-mod disclaimer and review any direct asset use before publishing.

## Character Fantasy

Working name: The Crawler.

The Crawler wins by arranging each turn into a rising sequence of card costs. Single cards are modest, but a well-planned turn becomes explosive as each correctly chained card increases the multiplier applied to its numbers.

The play pattern should feel like this:

- Start a chain from whatever cost the first played card has.
- Look for the exact next cost.
- Use draw, energy, cost control, and Wildcards to keep the chain alive.
- Turn one good sequence into a runaway turn.

## Core Mechanic: Cost Chain

Each turn tracks the cost of the last played card and the current chain multiplier.

Rules:

- The first card played each turn resolves at `1x` and starts a chain from its played cost.
- If the next played card costs exactly `last played cost + 1`, it resolves at `current multiplier + 1`.
- If the next played card does not meet that exact cost, it resolves at `1x` and starts a new chain from its played cost.
- The chain uses the card's actual played cost after cost modifications.
- X-cost cards use the amount of energy actually spent as their played cost.
- Only cards intentionally played by the player update the chain. Automatic follow-up effects, passive relic effects, and damage instances created by a card do not start or continue the chain.
- The chain resets at the start of each turn.

Example:

`0-cost card (1x) -> 1-cost card (2x) -> 2-cost card (3x) -> 1-cost card (1x) -> 2-cost card (2x)`

## Multiplier Scope

The multiplier affects all main numeric card effects unless a card explicitly says otherwise.

Affected examples:

- Damage
- Block
- Healing
- Card draw
- Energy gain
- Buff stacks gained
- Debuff stacks applied
- Generated card counts

Cards may opt out with explicit text such as "This card is not affected by chain multiplier" or "Only this card's damage is multiplied."

This intentionally allows unstable, high-ceiling turns. The role of card and relic design is to make those turns earned, not to prevent them completely.

## Build Routes

### Weapon Chain

The direct damage route. It uses attacks, multi-hit cards, and finishing attacks to convert high multipliers into lethal turns.

Design targets:

- Rewards clean `0 -> 1 -> 2 -> 3` or `1 -> 2 -> 3` chains.
- Includes some multi-hit cards, but watches for runaway scaling when multiplied.
- Has a few payoff cards that are weak at `1x` and excellent at `3x+`.

Risks:

- Damage can become too automatic if every attack is efficient before multiplier.
- Multi-hit cards need careful numbers because multiplier affects each numeric damage value.

### Item Chain

The consistency and engine route. It uses draw, energy, retain, cost changes, and card generation to keep chains going.

Design targets:

- Provides the tools that let strict chaining work in Slay the Spire's hand system.
- Makes the player feel clever for bridging awkward costs.
- Supports long turns without making infinite loops effortless.

Risks:

- Since multiplier affects draw and energy, this route can create infinite or near-infinite turns.
- Some draw or energy cards may need caps, exhaust, once-per-turn logic, or explicit multiplier exceptions.

### Curse Chain

The risky high-reward route. It uses harmful cards, self-inflicted debuffs, dead draws, or delayed penalties in exchange for stronger multiplier payoffs.

Design targets:

- Gives players a reason to accept awkward or dangerous cards.
- Can provide extra chain power, more draw, or large finishers if the player survives the downside.
- Should feel dark, weird, and very Vampire Crawlers.

Risks:

- If penalties are too soft, the route becomes the best route by default.
- If penalties are too hard, players will avoid it outside novelty runs.

## Wildcards

Wildcards are support cards that bend the chain rules. They should help the strict cost sequence remain fun without replacing the core mechanic.

Possible Wildcard effects:

- This card is treated as if its cost were `last played cost + 1`.
- The next card is treated as if it continued the chain.
- Preserve the current multiplier for one failed chain step.
- Copy the previous card's played cost.
- Change a card's cost in hand.
- Search or generate a card with the next required cost.

Design boundaries:

- Wildcards should be useful, but not required every turn.
- Most Wildcards should have modest direct output.
- The strongest Wildcards should exhaust, be rare, or carry a cost.
- Wildcards must be readable at a glance; chain manipulation can become confusing quickly.

## Starter Kit

The starting deck should teach the chain immediately.

Initial direction:

- Include at least one 0-cost starter card.
- Include simple 1-cost attack and block cards.
- Include one basic 2-cost card so the player can see a three-step chain early.
- Avoid giving too much draw in the starter deck.

Starter relic concept:

- Shows the current chain multiplier and the next required cost.
- May provide a small baseline benefit when a chain reaches `3x` for the first time each combat.

## Card Pool Shape

Target first full version:

- About 60-75 cards total.
- Roughly 25-30 Weapon Chain cards.
- Roughly 20-25 Item Chain cards.
- Roughly 15-20 Curse Chain cards.
- Wildcard effects distributed across all three routes, with the highest density in Item Chain.

Cost distribution matters more than usual. The pool should contain enough cards at 0, 1, 2, and 3 cost to make chains plausible, with 4+ cost reserved for rare payoffs or special cases.

## Relics

Target first full version:

- 1 starting relic.
- 8-12 character relics.

Relic themes:

- Reward reaching specific chain lengths.
- Improve the first card of a chain.
- Help find or create the next required cost.
- Add risk/reward for Curse Chain.
- Make Wildcards more effective without making them mandatory.

Avoid relics that permanently increase every chain multiplier without constraints, because they can flatten the character into pure number scaling.

## Balance Philosophy

The character should be allowed to have absurd turns. That is the point. Balance should focus on setup cost, consistency, and survivability rather than removing the high ceiling.

Good failure states:

- The player has big payoffs but cannot line up costs.
- The player has chain tools but not enough output.
- The player pushes Curse Chain too hard and pays for it.

Bad failure states:

- The hand contains no reasonable way to start or continue chains.
- The optimal play is always to ignore the chain and play cards normally.
- The optimal play is always an obvious infinite loop.

## Testing Plan

Early testing should answer these questions:

- Does the chain rule feel understandable after one combat?
- Are `0 -> 1 -> 2` and `1 -> 2 -> 3` chains both viable?
- Does multiplying draw and energy feel exciting without making every engine infinite?
- Do Wildcards feel like clever fixes rather than mandatory keys?
- Are Weapon, Item, and Curse routes all recognizable within a run?

## Out of Scope For v1

- Custom events.
- Custom potions.
- Full original art pass.
- Large narrative integration.
- Perfect public-release asset cleanup.

These can come after the core character is playable and fun.
