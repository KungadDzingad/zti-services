package net.wojtek.zti.models.request;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@NoArgsConstructor
@Getter
@Setter
public class CreateMessageRequest {
    private int senderId;
    private int receiverId;
    private String content;
}
