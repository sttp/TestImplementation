# TestImplementation

This contains skeleton code to assist in writing the specifications document.

## Layout

- **Sttp.WireProtocol** - Serialization and stream encoding for messages.
- **Sttp.Core** - Metadata and Data Value Objects.
- **Sttp.Publisher** - API layer that exists above wire protocol layer whose details are invisible to API user.
- **Sttp.Subscriber** - API layer that exists above wire protocol layer whose details are invisible to API user.

![dependency-graph.png](/docs/dependency-graph.png)